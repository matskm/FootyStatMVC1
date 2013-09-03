using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FootyStatMVC1.Models.FootyStat.SnapViewNS;
using FootyStatMVC1.Models.FootyStat.Filters;
using FootyStatMVC1.Models.FootyStat.Mediator.Colleagues;
using FootyStatMVC1.Models.FootyStat.Actions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FootyStatMVC1.Models.FootyStat.Actions.Constraints;
using FootyStatMVC1.Models.FootyStat.Mediator;
using FootyStatMVC1.Controllers.ConstraintViewModels;
using FootyStatMVC1.Models.FootyStat.Factory;
using FootyStatMVC1.Models.FootyStat.Factory.Inputs;
using FootyStatMVC1.Models.FootyStat.SnapViewCommand.Strategies;
using System.Reflection;
using FootyStatMVC1.Controllers.SessionWrapper;

using MCFactory = FootyStatMVC1.Models.FootyStat.Factory.MCFactoryWrapper;

namespace FootyStatMVC1.Controllers
{
    public class PlayerStatController : Controller
    {
        // Access to repositiory for this session
        // This approach is from LukLed, on StackOverflow question 5060804
        // Signature will be SessionWrapper.svd (both get and set are public)
        public ISessionWrapper SessionWrapper { get; set; }

        public PlayerStatController(ISessionWrapper sessionWrapper)
        {
            SessionWrapper = sessionWrapper;
        }



        // ViewModel
        public class PlayerStatViewModel
        {

            public PlayerStatViewModel(List<string> idx_season,
                                        List<string> idx_teamName = null,
                                        List<string> idx_playerName = null,

                                        string sel_season = null,
                                        string sel_teamName = null,
                                        string sel_playerName = null,
                                        CurrentPlayerStatsViewModel cpsVM = null
                                        )
            {
                this.seasonIndex = idx_season;
                this.teamNameIndex = idx_teamName;
                this.playerNameIndex = idx_playerName;

                // indicies cannot be null, so initialise them if they are
                if (seasonIndex == null) seasonIndex = new List<string>();
                if (teamNameIndex == null) teamNameIndex = new List<string>();
                if (playerNameIndex == null) playerNameIndex = new List<string>();




                this.selected_seasonIndex = sel_season;
                this.selected_teamNameIndex = sel_teamName;
                this.selected_playerNameIndex = sel_playerName;

                // Set the stats view model
                statsVM = cpsVM;

                // Defaults to inactive constraints
                haCVM = new HomeAwayCVM();
                gwCVM = new GameweekCVM();
                IsHome = false;
                mpCVM = new MinsPlayedCVM();

            }



            // Default constructor
            public PlayerStatViewModel()
            {

                init_index_lists();
            }

            void init_index_lists()
            {
                seasonIndex = new List<string>();
                teamNameIndex = new List<string>();
                playerNameIndex = new List<string>();
            }

            // Constraint section (must break this out into another class...)

            public HomeAwayCVM haCVM { get; set; }

            public GameweekCVM gwCVM { get; set; }

            public MinsPlayedCVM mpCVM { get; set; }

            public bool IsHome { get; set; }






            // Player stats view model
            public CurrentPlayerStatsViewModel statsVM { get; set; }



            // Season in?dex
            [DisplayName("Season")]
            public string selected_seasonIndex { get; set; }
            public List<string> seasonIndex { get; set; }
            // Get a SelectList version of the list
            public IEnumerable<SelectListItem> seasonIndex_select_list
            {
                get
                {
                    // Cunning lambda expression where both Text and Value are set to the same string
                    // and a SelectListItem created from this (later I may choose to implement a "Season" class which breaks generic-ness)
                    return seasonIndex.Select(d => new SelectListItem() { Text = d, Value = d });
                }
            }

            // teamNameIndex
            [DisplayName("Team Name")]
            public string selected_teamNameIndex { get; set; }
            public List<string> teamNameIndex { get; set; }
            // Get a SelectList version of the list
            public IEnumerable<SelectListItem> teamNameIndex_select_list
            {
                get
                {
                    // Cunning lambda expression where both Text and Value are set to the same string
                    // and a SelectListItem created from this (later I may choose to implement a "Season" class which breaks generic-ness)
                    return teamNameIndex.Select(d => new SelectListItem() { Text = d, Value = d });
                }
            }


            // playerNameIndex
            [DisplayName("Player Name")]
            public string selected_playerNameIndex { get; set; }
            public List<string> playerNameIndex { get; set; }
            // Get a SelectList version of the list
            public IEnumerable<SelectListItem> playerNameIndex_select_list
            {
                get
                {
                    // Cunning lambda expression where both Text and Value are set to the same string
                    // and a SelectListItem created from this (later I may choose to implement a "Season" class which breaks generic-ness)
                    return playerNameIndex.Select(d => new SelectListItem() { Text = d, Value = d });
                }
            }


            // Loop over the constraint properties
            public void loop_over_constraintVM(SnapViewDirector svd)
            {

                // Loop over properties in ViewModel looking for BaseConstraintViewModel objects
                foreach (var prop in typeof(PlayerStatViewModel).GetProperties())
                {
                    if (prop.GetValue(this) is BaseConstraintViewModel)
                    {
                        BaseConstraintViewModel bvm = (BaseConstraintViewModel)prop.GetValue(this);

                        // Generate a concrete ConstraintMC based on this viewmodel 
                        //  Two purposes for this:
                        //      i) Use it to search for an exisiting Constraint in the svd
                        //     ii) If this constraint is active in the viewmodel, this new MC will be attached to the svd
                        ConstraintMC cmc_vm = bvm.generate_ConstraintMC(svd);

                        // Search for ConstraintMC matching this one - look for underlying action type matching.
                        ConstraintMC cmc_svd = svd.get_matching_ConstraintMC(cmc_vm);

                        // If we found a ConstraintMC with the same underlying Constraint
                        // then detach it from the svd to prepare for the new one. 
                        // Or we don't need a constraint of this type at all if this constraint is disabled in the viewmodel
                        // In either case we need to detach it.
                        if (cmc_svd != null) svd.Detach(cmc_svd);

                        // Now the existing svd version is detached if found, we attach the new one if needed
                        if (bvm.active) svd.Attach(cmc_vm);

                    }//if BaseConstraintViewModel
                }//foreach
            }//method

        }// PlayerStatViewModel





        // Helper methods

        public SnapViewDirector get_svd()
        {
            //return FootyStatMVC1.FootyStatInit.get_svd();
            return SessionWrapper.svd;
        }

        public ActionResult Index()
        {
            return View();
        }

        string get_fieldChoice(string field_name)
        {
            //Field f = get_field(field_name);
            //if (f.projectedOut) return f.projectedVal;
            //else return null;
            
            // Get this via svd and IndexMC 
            List<string> idx = get_svd().get_index(field_name);
            if (idx.Count != 1)
            {
                // throw exception - this shouldn't be > 1, because the index should be degenerate.
                // if this does fire, its because we're looking for a "single valued" index (i.e., choice) but the index is 
                // not single valued (i.e., size>1)
                return null;
            }
            else
            {
                return idx[0];
            }
        }

        string get_seasonChoice()
        {
            return get_fieldChoice(FieldDictionary.fname_season);
        }

        string get_teamNameChoice()
        {
            return get_fieldChoice(FieldDictionary.fname_teamName);
        }

        string get_playerNameChoice()
        {
            return get_fieldChoice(FieldDictionary.fname_playerSurname);
        }


        

        // ***************** END HELPERS


        // Get version of the View
        public ActionResult SelectPlayer()
        {
            // This is the first page load - so initialise the (svd,snapview) pair here for this session.
            // This init is protected from executing >1 internally. But this method does get called twice.
            SessionWrapper.init_svd();
            
            UpdateIndex_CS cmd_strategy = new UpdateIndex_CS(get_svd());
            List<string> idx_list = cmd_strategy.execute(FieldDictionary.fname_season);
            
            return View(new PlayerStatViewModel(idx_list));
        }

        // Default Post version for rendering the html given the new viewModel
        [HttpPost]
        public ActionResult SelectPlayer(PlayerStatViewModel viewModel)
        {
            // This shouldn't fire with the current setup as submits are routed to other action methods.

            return View(viewModel);

        }


        // Test area for cleaner multiple submit framework

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "ChosenSeason")]
        public ActionResult ChosenSeason(PlayerStatViewModel viewModel) 
        {
            // This no longer valid way of handling this validation as need to redirect to PlayerStat
            // [SERVER SIDE VALIDATION SHOULD BE HANDLED HERE]
            //if (!ModelState.IsValid)
            //    return View(viewModel);

            
            UpdateIndexAndFilter_CS cmd_strategy = new UpdateIndexAndFilter_CS(get_svd());
            List<string> team_idx = cmd_strategy.execute(FieldDictionary.fname_teamName, FieldDictionary.fname_season, viewModel.selected_seasonIndex);

            




            return View("SelectPlayer", new PlayerStatViewModel(new List<string>(), 
                                                                team_idx, 
                                                                new List<string>(), 
                                                                viewModel.selected_seasonIndex, 
                                                                null, 
                                                                null)
                                                                );

        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "ChosenTeam")]
        public ActionResult ChosenTeam(PlayerStatViewModel viewModel)
        {

            //if (!ModelState.IsValid)
            //    return View(viewModel);

            //List<string> player_idx = getplayerNameIndex(viewModel.selected_teamNameIndex);

            UpdateIndexAndFilter_CS cmd_strategy = new UpdateIndexAndFilter_CS(get_svd());
            List<string> player_idx = cmd_strategy.execute(FieldDictionary.fname_playerSurname, FieldDictionary.fname_teamName, viewModel.selected_teamNameIndex);

            

            return View("SelectPlayer", new PlayerStatViewModel(new List<string>(), 
                                                                new List<string>(), 
                                                                player_idx, 
                                                                get_seasonChoice(), 
                                                                viewModel.selected_teamNameIndex, 
                                                                null)
                                                                );

        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "ChosenPlayer")]
        public ActionResult ChosenPlayer(PlayerStatViewModel viewModel)
        {

            //if (!ModelState.IsValid)
            //    return View(viewModel);

            //filter_on_playerName(viewModel.selected_playerNameIndex);

            Filter_CS cmd_strategy1 = new Filter_CS(get_svd());
            cmd_strategy1.execute(FieldDictionary.fname_playerSurname, viewModel.selected_playerNameIndex);

            UpdateAllStats_CS cmd_strategy2 = new UpdateAllStats_CS(get_svd());
          
            return View("SelectPlayer", new PlayerStatViewModel(
                                                                new List<string>(), 
                                                                new List<string>(), 
                                                                new List<string>(), 
                                                                
                                                                get_seasonChoice(), 
                                                                get_teamNameChoice(), 
                                                                viewModel.selected_playerNameIndex,
                                                                //get_statsVM()
                                                                cmd_strategy2.execute()
                                                                ));

        }

        


        [HttpPost]
        [MultipleButton(Name = "action", Argument = "ApplyConstraints")]
        public ActionResult ApplyConstraints(PlayerStatViewModel viewModel)
        {
            
            UpdateConstraintsAndAllStats_CS cmd_strategy = new UpdateConstraintsAndAllStats_CS(get_svd());
            
            return View("SelectPlayer", new PlayerStatViewModel(
                                        new List<string>(), 
                                        new List<string>(), 
                                        new List<string>(), 
                                        
                                        get_seasonChoice(), 
                                        get_teamNameChoice(), 
                                        get_playerNameChoice(),
                                        //get_statsVM()
                                        cmd_strategy.execute(viewModel)
                                        ));

        }





        public ActionResult testMethod()
        {

            return View();
        }

    }
}
